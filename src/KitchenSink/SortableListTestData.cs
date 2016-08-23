using Starcounter;
using System;

namespace KitchenSink
{
    static class SortableListTestData
    {
        public static bool Exists()
        {
            var exists = Db.SQL<People>("SELECT i FROM People i FETCH ?", 1).First;
            if (exists == null)
            {
                return false;
            }
            return true;
        }

        public static void DeleteAll()
        {
            Db.SlowSQL("DELETE FROM People");
        }

        public static int GetLastOrderNumber()
        {
            return Exists() ? (int)Db.SQL<long>("SELECT max(i.Ordering) FROM People i").First : 0;
        }

        public static void Create()
        {
            Db.Transact(() => {
                string[] test_people_list = new string[] { "Michael", "Jonas", "Mark", "David", "Jane", "Mary", "Paul", "John"};

                foreach (string item in test_people_list)
                {
                    
                    var people = new People()
                    {
                        Name = item,
                        Ordering = GetLastOrderNumber() + 1
                    };

                }

            });
        }
    }
}
