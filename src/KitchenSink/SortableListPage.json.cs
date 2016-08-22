using Starcounter;

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
            if (SortableListTestData.Exists())
            {
                Db.Transact(() => {
                    SortableListTestData.DeleteAll();
                });
            }
            SortableListTestData.Create();

            People = Db.SQL<People>("SELECT i FROM People i ORDER BY Ordering");

        }
    }
}
