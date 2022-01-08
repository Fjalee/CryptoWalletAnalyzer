using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using System;

namespace WebScraper
{
    public abstract class Parser
    {
        protected readonly string _unknownTokenImg;

        public Parser(string unknownTokenImg)
        {
            _unknownTokenImg = unknownTokenImg;
        }

        abstract public IElement ParseTxnTable(IHtmlDocument page);

        abstract public Transaction ParseTxnRow(IElement row);

        abstract protected string ParseImgSrc(IElement row);

        abstract protected string ParseToken(IElement row);

        abstract protected string ParseTxnHash(IElement row);

        abstract protected TokenValueInfo ParseValue(IElement row);

        protected void StepIfMatches(ref IElement current, string actualAttribute, string expectedAttribute, IElement nextStep)
        {
            if (expectedAttribute != actualAttribute)
            {
                throw new InvalidOperationException($"attribute {actualAttribute} was expected to be equal to {expectedAttribute}");
            }
            current = nextStep;
        }
    }
}
