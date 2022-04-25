// �Զ�����
namespace Common.RedisLock.Core
{
    /// <summary>
    /// �䵱�ض����͵� <see cref="IDistributedReaderWriterLock"/> ʵ���Ĺ����� ������������
    /// ������ע�볡���б� <see cref="IDistributedReaderWriterLock"/> ������ʹ�á�
    /// </summary>
    public interface IDistributedReaderWriterLockProvider
    {
        /// <summary>
        /// ʹ�ø����� <paramref name="name"/> ����һ�� <see cref="IDistributedReaderWriterLock"/> ʵ����
        /// </summary>
        IDistributedReaderWriterLock CreateReaderWriterLock(string name);
    }
}