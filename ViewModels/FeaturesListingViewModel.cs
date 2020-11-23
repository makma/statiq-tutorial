using System.Collections.Generic;

namespace StatiqTutorial
{
    public class FeaturesListingViewModel
    {
        public IReadOnlyList<Feature> Features { get; private set; }

        public FeaturesListingViewModel(List<Feature> features)
        {
            Features = features;
        }
    }
}
