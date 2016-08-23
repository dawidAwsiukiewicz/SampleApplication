using Starcounter;
using System;

namespace KitchenSink
{
    [Database]
    public class People
    {
        public string Name;
        public int Ordering;
    }

    partial class SortableListPage : Json
    {

        protected override void OnData()
        {
            base.OnData();
            Console.Out.WriteLine("DUPA");
            if (SortableListTestData.Exists())
            {
                Db.Transact(() =>
                {
                    SortableListTestData.DeleteAll();
                });
            }
            SortableListTestData.Create();
            
            People = Db.SQL<People>("SELECT i FROM People i ORDER BY i.Ordering");

           // OnChangedPosition(Db.SQL<People>("SELECT i FROM People i WHERE i.Ordering = ?", 5).First, 0);
        }



        protected void OnChangedPosition(People Item, int NewOrder)
        {

            if (NewOrder > Item.Ordering)
            {
                Db.Transact(delegate
                {
                    foreach (People item in Db.SQL<People>("SELECT i FROM People i WHERE i.Ordering <= ? AND i.Ordering > ?", NewOrder, Item.Ordering))
                    {
                        item.Ordering = item.Ordering - 1;
                    }
                });
            }
            else if (NewOrder < Item.Ordering)
            {
                Db.Transact(delegate
                {
                    foreach (People item in Db.SQL<People>("SELECT i FROM People i WHERE i.Ordering >= ? AND i.Ordering < ?", NewOrder, Item.Ordering))
                    {
                        item.Ordering = item.Ordering + 1;
                    }
                });
            }
            Db.Transact(delegate
            {
                Item.Ordering = NewOrder;
            });
         
        }


    }
    [SortableListPage_json.People]
    partial class SortableListPagePeopleElement : Json
    {

        void Handle(Input.OrderingUp action)
        {
            Db.Transact(delegate
            {
                foreach (People item in Db.SQL<People>("SELECT i FROM People i WHERE i.Ordering < ? ORDER BY i.Ordering DESC  LIMIT ?", Ordering, 1))
                {
                    int tmp_ordering = item.Ordering;
                    item.Ordering = (int) Ordering;
                    Ordering = tmp_ordering;
                }
            });
        }

        void Handle(Input.OrderingDown action)
        {
            Db.Transact(delegate
            {
                foreach (People item in Db.SQL<People>("SELECT i FROM People i WHERE i.Ordering > ?  ORDER BY i.Ordering ASC LIMIT ?", Ordering, 1))
                {
                    int tmp_ordering = item.Ordering;
                    item.Ordering = (int)Ordering;
                    Ordering = tmp_ordering;
                }
            });

        }
    }
}