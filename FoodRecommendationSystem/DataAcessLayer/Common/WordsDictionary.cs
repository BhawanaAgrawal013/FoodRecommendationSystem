namespace DataAcessLayer.Common
{
    public static class WordsDictionary
    {
        public static List<string> Negations = new List<string> {
            "not", "never", "no", "none", "nobody", "nothing", "neither", "nowhere", "hardly", "scarcely", "barely", "except",
            "doesn't", "didn't", "isn't", "wasn't", "aren't", "weren't", "can't", "couldn't", "won't", "wouldn't", "don't", "nor"
        };

        public static List<string> Intensifiers = new List<string> {
           "very", "extremely", "really", "so", "quite", "remarkably", "exceptionally", "tremendously", "hugely", "several", "various",
            "highly", "enormously", "totally", "absolutely", "utterly", "incredibly", "especially", "particularly", "awfully",
            "insanely", "many", "much", "every", "more", "most", "less", "least", "many", "few", "fewer", "some", "any", "each", "all", "none" };

        public static List<string> Conjunctions = new List<string> { 
            "but", "however", "although", "yet", "still", "though", "even", "if", "whereas", "while", "despite",
            "nonetheless", "nevertheless", "on the other hand", "instead", "alternatively", "otherwise"
        };

        public static List<string> Stopwords = new List<string>
        {
             "the", "is", "was", "a", "and", "to", "in", "that", "of", "for", "on", "with", "as", "by", "at", "from",
            "about", "into", "during", "including", "until", "against", "among", "throughout", "despite", "towards",
            "upon", "concerning", "through", "before", "after", "above", "below", "since", "without", "within", "along",
            "following", "across", "behind", "beyond", "plus", "up", "out", "around", "down", "off", "between",
            "under", "near", "over", "during", "before", "after", "to", "from", "in", "out", "up", "down", "with",
            "then", "once", "only", "just", "also", "too", "so", "quite", "really", "then", "well",
            "both", "either", "one", "two", "three", "such", "same", "different", 
            "other", "another", "last", "next", "first", "second", "third", "certain", "sure", "various"
        };

        public static Dictionary<string, int> SentimentWords = new Dictionary<string, int>
        {
            // Normal words
            {"good", 2}, {"hated", -3}, {"loved", 3}, {"happy", 3}, {"joy", 3}, {"excellent", 4}, {"great", 3}, {"positive", 2}, {"fortunate", 2}, {"correct", 1}, {"superior", 2},
            {"bad", -2}, {"sad", -3}, {"pain", -3}, {"terrible", -4}, {"poor", -3}, {"negative", -2}, {"unfortunate", -2}, {"wrong", -1}, {"inferior", -2},
            {"horrible", -4}, {"wonderful", 4}, {"awful", -4}, {"fantastic", 4}, {"amazing", 4}, {"love", 3}, {"like", 2}, {"hate", -3},
            {"dislike", -2}, {"enjoy", 3}, {"disappointed", -3}, {"satisfied", 3}, {"unsatisfied", -3}, {"remarkable", 4}, {"mediocre", -2}, {"awesome", 4},
            {"terrific", 4}, {"marvelous", 4}, {"fabulous", 4}, {"pleasant", 3}, {"nice", 2}, {"decent", 2}, {"average", 0}, {"boring", -2},
            {"dull", -2}, {"interesting", 2}, {"fascinating", 3}, {"engaging", 3}, {"entertaining", 3}, {"delightful", 4}, {"charming", 3}, {"annoying", -3},
            {"frustrating", -3}, {"horrific", -4}, {"appalling", -4}, {"disheartening", -3}, {"pleasing", 3}, {"gratifying", 3}, {"inspiring", 4}, {"unpleasant", -3}, {"hot", 2},

            // Positive food-related words
            {"delicious", 4}, {"tasty", 3}, {"flavorful", 3}, {"yummy", 3}, {"scrumptious", 4}, {"savory", 3}, {"mouthwatering", 4}, {"delectable", 4},
            {"appetizing", 3}, {"fresh", 3}, {"well-seasoned", 3}, {"rich", 2},

            // Negative food-related words
            {"bland", -3}, {"tasteless", -6}, {"overcooked", -3}, {"undercooked", -3}, {"burnt", -4}, {"soggy", -3}, {"greasy", -3}, {"dry", -3},
            {"stale", -3}, {"rubbery", -3}, {"flavorless", -3}, {"unappetizing", -3}, {"spoiled", -4},
            {"inedible", -4}, {"disgusting", -4}, { "not worth having", -6}, { "extremely bad experience", -6},
            
            // Neutral or context-specific words
            {"salt", 0}, {"sugar", 0}, {"butter", 0}, {"oil", 0}, {"bread", 0}, {"water", 0}, {"spice", 0}, {"meat", 0},
            {"fish", 0}, {"chicken", 0}, {"vegetable", 0}, {"fruit", 0}, {"alright", 0}, {"okay", 0}, {"fine", 0}
        };
    }
}
