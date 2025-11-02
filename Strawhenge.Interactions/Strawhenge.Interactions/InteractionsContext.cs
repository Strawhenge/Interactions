using System;
using Strawhenge.Common.Logging;

namespace Strawhenge.Interactions
{
    public class InteractionsContext
    {
        readonly ILogger _logger;

        public InteractionsContext(ILogger logger)
        {
            _logger = logger;
        }

        bool _isValid = true;

        public event Action Interrupted;

        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid == value)
                    return;

                _isValid = value;
                _logger.LogInformation($"Interaction context valid: {_isValid}");

                if (!_isValid)
                    Interrupt();
            }
        }

        public void Interrupt()
        {
            _logger.LogInformation("Interactions context interrupted.");
            Interrupted?.Invoke();
        }
    }
}