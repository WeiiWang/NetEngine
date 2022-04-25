// �Զ�����
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RedisLock.Core
{
    /// <summary>
    /// <see cref="IDistributedSemaphoreProvider" /> ����������������
    /// </summary>
    public static class DistributedSemaphoreProviderExtensions
    {
        /// <summary>
        /// �൱�ڵ��� <see cref="IDistributedSemaphoreProvider.CreateSemaphore(string, int)" /> Ȼ��
        /// <�μ� cref="IDistributedSemaphore.TryAcquire(TimeSpan, CancellationToken)" />��
        /// </summary>
        public static IDistributedSynchronizationHandle? TryAcquireSemaphore(this IDistributedSemaphoreProvider provider, string name, int maxCount, TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            (provider ?? throw new ArgumentNullException(nameof(provider))).CreateSemaphore(name, maxCount).TryAcquire(timeout, cancellationToken);

        /// <summary>
        /// �൱�ڵ��� <see cref="IDistributedSemaphoreProvider.CreateSemaphore(string, int)" /> Ȼ��
        /// <see cref="IDistributedSemaphore.Acquire(TimeSpan?, CancellationToken)" />.
        /// </summary>
        public static IDistributedSynchronizationHandle AcquireSemaphore(this IDistributedSemaphoreProvider provider, string name, int maxCount, TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            (provider ?? throw new ArgumentNullException(nameof(provider))).CreateSemaphore(name, maxCount).Acquire(timeout, cancellationToken);

        /// <summary>
        /// �൱�ڵ��� <see cref="IDistributedSemaphoreProvider.CreateSemaphore(string, int)" /> Ȼ��
        /// <�μ� cref="IDistributedSemaphore.TryAcquireAsync(TimeSpan, CancellationToken)" />��
        /// </summary>
        public static ValueTask<IDistributedSynchronizationHandle?> TryAcquireSemaphoreAsync(this IDistributedSemaphoreProvider provider, string name, int maxCount, TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            (provider ?? throw new ArgumentNullException(nameof(provider))).CreateSemaphore(name, maxCount).TryAcquireAsync(timeout, cancellationToken);

        /// <summary>
        /// �൱�ڵ��� <see cref="IDistributedSemaphoreProvider.CreateSemaphore(string, int)" /> Ȼ��
        /// <�μ� cref="IDistributedSemaphore.AcquireAsync(TimeSpan?, CancellationToken)" />��
        /// </summary>
        public static ValueTask<IDistributedSynchronizationHandle> AcquireSemaphoreAsync(this IDistributedSemaphoreProvider provider, string name, int maxCount, TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            (provider ?? throw new ArgumentNullException(nameof(provider))).CreateSemaphore(name, maxCount).AcquireAsync(timeout, cancellationToken);
    }
}