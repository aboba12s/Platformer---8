using UnityEngine;
using Object = UnityEngine.Object;

namespace YOUR_NAME
{
    public static class PlatformerExtensions
    {
        public static int ToMilliseconds(this float seconds)
        {
            return (int)(seconds * 1000f);
        }

        public static void PlayOneShot(this AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f, float tail = 1f)
        {
            if (!clip) return;
            
            var go = new GameObject("Sound")
            {
                transform =
                {
                    position = position
                }
            };

            var audio = go.AddComponent<AudioSource>();
            
            audio.clip = clip;
            audio.volume = volume;
            audio.pitch = pitch;
            
            audio.Play();
            Object.Destroy(go, clip.length + tail);
        }
    }
}
