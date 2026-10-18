using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public interface ISelectableObject
{
    public string Name { get; }
    public CustomStringBuilder Characteristics { get; }
    public string Description { get; }
}
