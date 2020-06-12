using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Corum.Models.ViewModels.Orders;

namespace CorumAdminUI.CustomBinders
{
    public class OrdersBaseViewModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var typeValue = bindingContext.ValueProvider.GetValue("ModelType");

            var TypeName = (string)typeValue.ConvertTo(typeof(string));

            var type = Type.GetType(TypeName + ", Corum.Models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            if (!typeof(OrderBaseViewModel).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("Bad derrived type for OrderBaseViewModel");
            }
            var model = Activator.CreateInstance(type);

            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);

            return model;

        }

        
    }
}