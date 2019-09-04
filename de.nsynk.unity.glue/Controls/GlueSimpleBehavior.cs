using System;
using UnityEngine;

namespace Glue
{
  /// <summary>
  /// Base class for receiving values via Glue.
  /// GlueSimpleBehaviour compared to GlueBehaviour only provides
  /// the API for receiving and sending Glue values.
  /// </summary>
  public abstract class GlueSimpleBehaviour : MonoBehaviour
  {
    /// <summary>
    /// Convenience method for retrieving a value from Glue
    /// </summary>
    /// <param name="key">
    /// A key to match values on the other side (vvvv)
    /// </param>
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
    public T GlueValue<T>(string key, T fallback)
    {
      return DataPool.GlueFor<T>(key, fallback);
    }

    /// <summary>
    /// Convenience method for sending a value over Glue.
    /// [Currently not used]
    /// </summary>
    /// <param name="key">
    /// A key to match values on the other side (vvvv)
    /// </param>
    /// <param name="value">
    /// The value to send over Glue
    /// </param>
    /// <example>Example usage:
    /// <code>
    /// float sendValue = 0.5f;
    /// string key = "Value From Unity";
    /// GlueSend(key, sendValue);
    /// </code>
    /// </example>
    public void GlueSend<T>(string key, T value)
    {
      DataPool.SendFrame.Add(key, ref value);
    }
  }
}
