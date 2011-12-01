namespace Wish.Views
{
    public static class ViewId
    {
        private static int _id;
        public static int Next { 
            get { return _id++; }
        }
    }
}
