using UnityEngine;

public static class TagUtilities
{
    public static bool HasTag(GameObject obj, TagType tag)
    {
        MultiTag multiTag = obj.GetComponent<MultiTag>();
        return multiTag != null && multiTag.HasTag(tag);
    }

    public static bool HasAnyTag(GameObject obj, TagType[] tags)
    {
        MultiTag multiTag = obj.GetComponent<MultiTag>();
        if (multiTag != null)
        {
            foreach (TagType tag in tags)
            {
                if (HasTag(obj, tag)) return true;
            }
        }
        return false;
    }
}
