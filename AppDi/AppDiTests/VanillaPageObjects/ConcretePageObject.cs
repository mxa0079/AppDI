using AppDi;

namespace AppDiTests.VanillaPageObjects
{
    /// <summary>
    /// A vanilla helper class to enable unit testing.
    /// </summary>
    public class ConcretePageObject : PageObject
    {

    }

    /// <summary>
    /// A vanilla helper class to enable unit testing.
    /// </summary>
    public class VanillaPageObject : PageObject
    {

    }

    /// <summary>
    /// A class that should never be registered dinamically as a page object
    /// </summary>
    public class NotAPageObject{
    }
}
