namespace StatiqTutorial
{
    public class HomeViewModel
    {
        public string Title { get; private set; }

        public string Content { get; private set; }

        public HomeViewModel(Home home)
        {
            Title = home.Title;
            Content = home.Content;
        }

        public HomeViewModel(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
