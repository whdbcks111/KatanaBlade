using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Forms.Core;

namespace .Application.Form.FieldType
{
    public class Review : Umbraco.Forms.Core.FieldType
    {
        public Review()
        {
            //Provider
            this.Id = new Guid("6da341e1-6cf3-4ed8-91fc-413dab7fe3ff");
            this.Name = "Form Review";
            this.Description = "Show submitted items in the form for review.";
            this.Icon = "icon-notepad-alt";
            this.DataType = FieldDataType.String;
            this.SortOrder = 10;
        }
    }
}