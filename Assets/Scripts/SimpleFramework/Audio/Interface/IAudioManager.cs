using SimpleFramework.Aduio;
using  SimpleFramework.Common;
namespace SimpleFramework.Audio
{
    public interface IAudioManager : IManager
    {
        AudioEmitter Get();

        void ReturnToPool(AudioEmitter soundEmitter);

        bool CanPlayeSound(AudioData audioData);

        void EnqueueFrequentSound(AudioEmitter soundEmitter);
    }
}