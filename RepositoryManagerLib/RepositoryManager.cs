using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using Newtonsoft.Json;
using RepositoryManagerLib.Data;
using Microsoft.EntityFrameworkCore;


namespace RepositoryManagerLib
{
    public interface IRepositoryManager
    {
        void Initialize();
        void Register(string itemName, string ItemContent, int itemType);
        string Retrieve(string itemName);
        int GetType(string itemName);
        void Deregister(string itemName);
    }

    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private bool _initialized = false;
        private readonly SemaphoreSlim _initLock = new(1, 1);

        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            _initLock.Wait();
            try
            {
                if (_initialized) return;
                _context.Database.Migrate();
                _initialized = true;
            }
            finally
            {
                _initLock.Release();
            }
        }

        public void Register(string itemName, string itemContent, int itemType)
        {
            if (string.IsNullOrWhiteSpace(itemName))
                throw new ArgumentException("ItemName cannot be empty");

            if (itemType != 1 && itemType != 2)
                throw new ArgumentException("Invalid itemType, must be 1 (JSON) or 2 (XML)");

            if (itemType == 1) ValidateJson(itemContent);
            else ValidateXml(itemContent);

            var exists = _context.RepositoryItems.Find(itemName);
            if (exists != null)
                throw new InvalidOperationException($"Item with name '{itemName}' already exists.");

            var newItem = new Data.RepositoryItem
            {
                ItemName = itemName,
                ItemContent = itemContent,
                ItemType = itemType
            };

            _context.RepositoryItems.Add(newItem);
            _context.SaveChanges();
        }

        public string Retrieve(string itemName)
        {
            var item = _context.RepositoryItems.Find(itemName);
            if (item == null) throw new KeyNotFoundException($"Item '{itemName}' not found.");
            return item.ItemContent;
        }

        public int GetType(string itemName)
        {
            var item = _context.RepositoryItems.Find(itemName);
            if (item == null) throw new KeyNotFoundException($"Item '{itemName}' not found.");
            return item.ItemType;
        }

        public void Deregister(string itemName)
        {
            var item = _context.RepositoryItems.Find(itemName);
            if (item == null) throw new KeyNotFoundException($"Item '{itemName}' not found.");
            _context.RepositoryItems.Remove(item);
            _context.SaveChanges();
        }

        private void ValidateJson(string json)
        {
            try
            {
                JsonConvert.DeserializeObject(json);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Invalid JSON content", ex);
            }
        }

        private void ValidateXml(string xml)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
            }
            catch (XmlException ex)
            {
                throw new ArgumentException("Invalid XML content", ex);
            }
        }
    }
}