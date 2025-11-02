using System;

namespace Strawhenge.Interactions
{
    public class InteractionsContext
    {
        bool _isValid;

        public event Action Invalidated;

        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid == value)
                    return;

                _isValid = value;

                if (!_isValid)
                    Invalidated?.Invoke();
            }
        }
    }
}