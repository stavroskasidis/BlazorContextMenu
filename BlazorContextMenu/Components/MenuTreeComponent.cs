using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorContextMenu
{
    public abstract class MenuTreeComponent : ComponentBase, IDisposable
    {
        [CascadingParameter(Name = "ParentComponent")]
        public MenuTreeComponent ParentComponent { get; protected set; }
        protected List<MenuTreeComponent> _childComponents = new List<MenuTreeComponent>();

        public IReadOnlyList<MenuTreeComponent> GetChildComponents()
        {
            return _childComponents.AsReadOnly();
        }

        protected void RegisterChild(MenuTreeComponent childComponent)
        {
            _childComponents.Add(childComponent);
        }

        protected void RemoveChild(MenuTreeComponent childComponent)
        {
            _childComponents.Remove(childComponent);
        }

        protected override void OnInit()
        {
            base.OnInit();
            if(ParentComponent != null)
            {
                ParentComponent.RegisterChild(this);
                ParentComponent.StateHasChanged();
            }
        }

        public virtual void Dispose()
        {
            if (ParentComponent != null)
            {
                ParentComponent.RemoveChild(this);
            }
        }
    }
}
