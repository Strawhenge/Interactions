using Strawhenge.Common.Unity;
using UnityEngine;

namespace Strawhenge.Interactions.Unity
{
    public class InteractionsContextScript : MonoBehaviour
    {
        [SerializeField] LoggerScript _logger;

        InteractionsContext _context;

        public InteractionsContext Context => _context ??= CreateContext();

        void Awake()
        {
            _context ??= CreateContext();
        }

        InteractionsContext CreateContext()
        {
            var logger = _logger != null
                ? _logger.Logger
                : new UnityLogger(gameObject);

            return new InteractionsContext(logger);
        }
    }
}