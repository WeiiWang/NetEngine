// �Զ�����
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RedisLock.Core
{
    /// <summary>
    /// <see cref="IDistributedLockProvider" /> ����������������
    /// </summary>
    public static class DistributedLockProviderExtensions
    {
        /// <summary>
        /// �൱�ڵ��� <see cref="IDistributedLockProvider.CreateLock(string)" /> Ȼ��
        /// <�μ� cref="IDistributedLock.TryAcquire(TimeSpan, CancellationToken)" />��
        /// </summary>
        public static IDistributedSynchronizationHandle? TryAcquireLock(this IDistributedLockProvider provider, string name, TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            (provider ?? throw new ArgumentNullException(nameof(provider))).CreateLock(name).TryAcquire(timeout, cancellationToken);

        /// <summary>
        /// �൱�ڵ��� <see cref="IDistributedLockProvider.CreateLock(string)" /> Ȼ��
        /// <see cref="IDistributedLock.Acquire(TimeSpan?, CancellationToken)" />��
        /// </summary>
        public static IDistributedSynchronizationHandle AcquireLock(this IDistributedLockProvider provider, string name, TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            (provider ?? throw new ArgumentNullException(nameof(provider))).CreateLock(name).Acquire(timeout, cancellationToken);

        /// <summary>
        /// �൱�ڵ��� <see cref="IDistributedLockProvider.CreateLock(string)" /> Ȼ��
        /// <�μ� cref="IDistributedLock.TryAcquireAsync(TimeSpan, CancellationToken)" />��
        /// </summary>
        public static ValueTask<IDistributedSynchronizationHandle?> TryAcquireLockAsync(this IDistributedLockProvider provider, string name, TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            (provider ?? throw new ArgumentNullException(nameof(provider))).CreateLock(name).TryAcquireAsync(timeout, cancellationToken);

        /// <summary>
        /// �൱�ڵ��� <see cref="IDistributedLockProvider.CreateLock(string)" /> Ȼ��
        /// <�μ� cref="IDistributedLock.AcquireAsync(TimeSpan?, CancellationToken)" />��
        /// </summary>
        public static ValueTask<IDistributedSynchronizationHandle> AcquireLockAsync(this IDistributedLockProvider provider, string name, TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            (provider ?? throw new ArgumentNullException(nameof(provider))).CreateLock(name).AcquireAsync(timeout, cancellationToken);
    }
}