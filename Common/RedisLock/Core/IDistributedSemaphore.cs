using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RedisLock.Core
{
    /// <summary>
    /// һ��ͬ��ԭ���������Դ�����Ĺؼ����ֵķ�������Ϊ�̶������Ĳ����߳�/���̡�
    /// �� <see cref="Semaphore"/> ���бȽϡ�
    /// </summary>
    public interface IDistributedSemaphore
    {
        /// <summary>
        /// Ψһ��ʶ�ź���������
        /// </summary>
        string Name { get; }

        /// <summary>
        /// �ź������õ����Ʊ������������ͬʱ��ȡ�ź����Ľ�������
        /// </summary>
        int MaxCount { get; }

        /// <summary>
        /// ����ͬ����ȡ�ź���Ʊ֤�� �÷���
        /// <code>
        ///     using (var handle = mySemaphore.TryAcquire(...))
        ///     {
        ///         if (handle != null) { /* we have the ticket! */ }
        ///     }
        ///     // dispose releases the ticket if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> �������ͷ�Ʊ֤��ʧ��ʱΪ��</returns>
        IDistributedSynchronizationHandle? TryAcquire(TimeSpan timeout = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// ͬ����ȡ�ź���Ʊ֤��������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �÷���
        /// <code>
        ///     using (mySemaphore.Acquire(...))
        ///     {
        ///         /* we have the ticket! */
        ///     }
        ///     // dispose releases the ticket
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> ���������ͷ�Ʊ֤</returns>
        IDistributedSynchronizationHandle Acquire(TimeSpan? timeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// �����첽��ȡ�ź���Ʊ֤�� �÷���
        /// <code>
        ///     await using (var handle = await mySemaphore.TryAcquireAsync(...))
        ///     {
        ///         if (handle != null) { /* we have the ticket! */ }
        ///     }
        ///     // dispose releases the ticket if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> �������ͷ�Ʊ֤��ʧ��ʱΪ��</returns>
        ValueTask<IDistributedSynchronizationHandle?> TryAcquireAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// �첽��ȡ�ź���Ʊ֤��������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �÷���
        /// <code>
        ///     await using (await mySemaphore.AcquireAsync(...))
        ///     {
        ///         /* we have the ticket! */
        ///     }
        ///     // dispose releases the ticket
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> ���������ͷ�Ʊ֤</returns>
        ValueTask<IDistributedSynchronizationHandle> AcquireAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    }
}
