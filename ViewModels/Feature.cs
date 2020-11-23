using System.Security;

namespace StatiqTutorial
{
    public class Feature
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        public string Slug { get; private set; }

        public Feature(string title, string description, string slug)
        {
            Title = title;
            Description = description;
            Slug = slug;
        }
    }
}
