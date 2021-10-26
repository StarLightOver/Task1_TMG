using System.Collections.Generic;

namespace Task1_TMG
{
    public interface IParser
    {
        (IEnumerable<int>, IEnumerable<string>) ParseIdsFromText(string parsingLine);

    }
}