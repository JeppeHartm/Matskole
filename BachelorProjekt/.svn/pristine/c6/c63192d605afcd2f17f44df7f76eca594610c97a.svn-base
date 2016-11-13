using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Game2.Component.Subcomponents
{
    public class OutputSubComponentModel : SuperComponentModel
    {
        public OutputSubComponentModel(Component2oModel parent, double sizemod): base(sizemod)
        {
            this.inLimit = 1;
            this.In[0] = parent;
        }
        public override int GetOut()
        {
            if (this == ((Component2oModel)In[0]).Primary)
            {
                return ((Component2oModel)In[0]).GetOut();
            }
            else
            {
                return ((Component2oModel)In[0]).GetSecondaryOut();
            }
            
        }
    }
}
