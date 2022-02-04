using System.Reflection;
using System.Windows.Forms;

namespace Inverse.Windows
{
    public static class PanelExtensions
    {
        public static void SetDoubleBuffered(this Panel panel)
        {
            typeof(Panel).InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               panel,
               new object[] { true });
        }
    }
}
