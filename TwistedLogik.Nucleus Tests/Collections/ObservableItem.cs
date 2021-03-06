﻿using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.NucleusTests.Collections
{
    public class ObservableItem : INotifyPropertyChanged
    {
        public String PropertyA
        {
            get { return propertyA; }
            set
            {
                if (propertyA != value)
                {
                    propertyA = value;
                    OnPropertyChanged("PropertyA");
                }
            }
        }

        public String PropertyB
        {
            get { return propertyB; }
            set
            {
                if (propertyB != value)
                {
                    propertyB = value;
                    OnPropertyChanged("PropertyB");
                }
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that was changed. If all of the object's properties have
        /// changed, this value can be either <see cref="String.Empty"/> or <c>null</c>.</param>
        protected virtual void OnPropertyChanged(String propertyName)
        {
            var temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, propertyName);
            }
        }

        // Property values.
        private String propertyA;
        private String propertyB;
    }
}
