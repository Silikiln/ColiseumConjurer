using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[XmlParse("Test")]
public class Test {
    static Test() {
        Debug.Log("Loading test files");
        List<Test> loadTest = XmlParseAttribute.ReadFileIntoList<Test>("Assets/Test.xml");
        foreach (Test t in loadTest)
            Debug.Log(t.ToString());
    }

    public enum TestEnum { Have, HaveNot, Neither }

    [XmlParse("SubTest")]
    public class SubTest
    {
        [XmlParse("Reason")]
        public string Reason { get; private set; }

        public override string ToString()
        {
            return "SubTest: " + Reason;
        }
    }

    [XmlParse("Name")]
    public string Name { get; private set; }

    [XmlParse("Description")]
    public string Description { get; private set; }

    [XmlParse("Value")]
    public int Value { get; private set; }

    [XmlParse("Enum")]
    public TestEnum Enum { get; private set; }

    [XmlParse("SubTests")]
    public List<SubTest> SubTests
    {
        get; private set;
    }

    [XmlParse("Items")]
    public List<int> Items { get; private set; }

    public override string ToString()
    {
        string subtests = "Subtests: " + SubTests.Count;
        foreach (SubTest sub in SubTests)
            subtests += "\n   " + sub.ToString();

        string items = "Items: " + Items.Count;
        foreach (int item in Items)
            items += "\n   " + item;

        return string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", Name, Description, Value, Enum, subtests, items);
    }
}
