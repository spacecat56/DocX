using System.Xml.Linq;

namespace Novacode

{
    public abstract class AbstractField : DocXElement
    {
        //public AbstractField(Document doc) : base(doc, null) { }

        //public XElement Xml { get; internal set; }

        /// <summary>
        /// Wrap the supplied arbitrary text of the field, in the field begin and
        /// end markers in a run. 
        /// </summary>
        /// <param name="fieldText">the field Id and any parameters needed, NO CHECKING is done</param>
        /// <param name="fieldContent"></param>
        /// <returns>XML with the run representing the field</returns>
        internal XElement Build(string fieldText, string fieldContent = null)
        {
            // to unravel the nesting, build the inner parts in an array first
            object[] parts = new object[(fieldContent==null)?3:5];
            int next = 0;

            parts[next++] = new XElement
            (
                XName.Get("r", DocX.w.NamespaceName),
                new XElement
                (
                    XName.Get("fldChar", DocX.w.NamespaceName),
                    new XAttribute(DocX.w + "fldCharType", "begin")
                )
            );


            parts[next++] = new XElement
            (
                XName.Get("r", DocX.w.NamespaceName),
                new XElement
                (
                    XName.Get("instrText", DocX.w.NamespaceName),
                    new XAttribute(XNamespace.Xml + "space", "preserve"),
                    fieldText
                )
            );

            // additional text e.g. for a hyperlink inserted for the field
            // entails two runs, one for a separator and the second for the text
            if (fieldContent != null)
            {
                parts[next++] = new XElement
                (
                    XName.Get("r", DocX.w.NamespaceName),
                    new XElement
                    (
                        XName.Get("fldChar", DocX.w.NamespaceName),
                        new XAttribute(DocX.w + "fldCharType", "separate")
                    )
                );
                parts[next++] = new XElement
                (
                    XName.Get("r", DocX.w.NamespaceName),
                    new XElement
                    (
                        XName.Get("t", DocX.w.NamespaceName),
                        fieldContent
                    )
                );
            }

            parts[next] = new XElement
            (
                XName.Get("r", DocX.w.NamespaceName),
                new XElement
                (
                    XName.Get("fldChar", DocX.w.NamespaceName),
                    new XAttribute(DocX.w + "fldCharType", "end")
                )
            );

            // then wrap them all up in a run, and we are done
            XElement xe = new XElement
            (
                XName.Get("r", DocX.w.NamespaceName),
                parts
            );

            return Xml = xe;
        }

        public abstract AbstractField Build();

        protected AbstractField(DocX document, XElement xml) : base(document, xml) { }
    }
}
