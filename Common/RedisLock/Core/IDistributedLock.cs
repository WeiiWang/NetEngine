using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RedisLock.Core
{
    /// <summary>
    /// ����ͬ��ԭ�������Э������Դ�����ؼ�����ķ���
    /// ����̻�ϵͳ�� ���ķ�Χ�͹���ȡ�����ض���ʵ��
    /// </summary>
    public interface IDistributedLock
    {
        /// <summary>
        /// Ψһ��ʶ��������
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ����ͬ����ȡ���� �÷���
        /// <code>
        ///     using (var handle = myLock.TryAcquire(...))
        ///     {
        ///         if (handle != null) { /* we have the lock! */ }
        ///     }
        ///     // dispose releases the lock if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> �������ͷ�������ʧ��ʱΪ��</returns>
        IDistributedSynchronizationHandle? TryAcquire(TimeSpan timeout = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// ͬ����ȡ����������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �÷���
        /// <code>
        ///     using (myLock.Acquire(...))
        ///     {
        ///         /* we have the lock! */
        ///     }
        ///     // dispose releases the lock
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ��<see cref="IDistributedSynchronizationHandle"/>���������ͷ���</returns>
        IDistributedSynchronizationHandle Acquire(TimeSpan? timeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// �����첽��ȡ���� �÷���
        /// <code>
        ///     await using (var handle = await myLock.TryAcquireAsync(...))
        ///     {
        ///         if (handle != null) { /* we have the lock! */ }
        ///     }
        ///     // dispose releases the lock if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> �������ͷ�������ʧ��ʱΪ��</returns>
        ValueTask<IDistributedSynchronizationHandle?> TryAcquireAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// �첽��ȡ����������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �÷���
        /// <code>
        ///     await using (await myLock.AcquireAsync(...))
        ///     {
        ///         /* we have the lock! */
        ///     }
        ///     // dispose releases the lock
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ��<see cref="IDistributedSynchronizationHandle"/>���������ͷ���</returns>
        ValueTask<IDistributedSynchronizationHandle> AcquireAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    }
}
