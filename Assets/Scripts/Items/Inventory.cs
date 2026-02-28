using System;
using System.Collections.Generic;

namespace LoxQuest3D.Items
{
    [Serializable]
    public sealed class Inventory
    {
        public List<ItemStack> items = new();

        public int GetCount(ItemId id)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id == (int)id)
                    return items[i].count;
            }
            return 0;
        }

        public void Add(ItemId id, int count)
        {
            if (id == ItemId.None || count <= 0) return;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id == (int)id)
                {
                    items[i].count += count;
                    return;
                }
            }
            items.Add(new ItemStack { id = (int)id, count = count });
        }

        public bool TryRemove(ItemId id, int count)
        {
            if (id == ItemId.None || count <= 0) return false;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].id != (int)id) continue;
                if (items[i].count < count) return false;
                items[i].count -= count;
                if (items[i].count <= 0)
                    items.RemoveAt(i);
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public sealed class ItemStack
    {
        public int id;
        public int count;
    }
}

