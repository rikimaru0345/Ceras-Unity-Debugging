using System;
using UnityEngine;

namespace Glue
{
  /// <summary>
  /// Base class for receiving values via Glue
  /// </summary>
  public abstract class GlueBehaviour : MonoBehaviour
  {
    /// <summary>
    /// The key is used to match values from vvvv to Unity
    /// </summary>
    public string key;

    /// <summary>
    /// overwrite is a toggle that should be used if a Glue receiving
    /// value should be overwritten.
    /// </summary>
    /// <example>e.g.:
    /// <code>
    /// float fallback = 2.0f;
    /// float glueValue = GlueValue(fallback);
    /// if (overwrite)
    /// {
    ///    // do something to overwrite the controlled parameter
    /// }
    /// </code>
    /// </example>
    public bool overwrite = false;

    /// <summary>
    /// Convenience method for retrieving a value from Glue
    /// </summary>
    /// <param name="fallback">
    /// A fallback array, if no value could be found for the given
    /// key.
    /// </param>
    /// <returns>
    /// The value with type T
    /// </returns>
    /// <example>Example usage:
    /// <code>
    /// float[] fallback = new float[1]{ 2.0f };
    /// float[] glueValue = GlueValue(fallback);
    /// </code>
    /// </example>
    public T GlueValue<T>(T fallback)
    {
      return DataPool.GlueFor<T>(key, fallback);
    }

    public void GlueSend<T>(string key, T value)
    {
      DataPool.SendFrame.Add(key, ref value);
    }
  }
}
