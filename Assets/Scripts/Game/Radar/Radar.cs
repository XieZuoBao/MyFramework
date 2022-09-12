namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Radar", 40)]
    public class Radar : MaskableGraphic
    {
        public float radius = 200f;
        [SerializeField]
        [Range(0f, 1f)]
        private float[] values = new float[5];

        /// <summary>
        /// ���ñ���
        /// </summary>
        /// <param name="count">Ĭ��Ϊ5���Σ���С����Ϊ3</param>
        public void SetEdges(int count = 5)
        {
            values = new float[count];
            SetVerticesDirty();
        }

        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="index">����</param>
        /// <param name="value">��ֵ����Χ: [0f,1f] ��</param>
        public void UpdateValue(int index, float value)
        {
            if (CheckIndexValidity(index))
            {
                value = Mathf.Clamp(value, 0f, 1f);
                values[index] = value;
                SetVerticesDirty();
            }
        }

        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="index">����</param>
        /// <returns>���û�л�ȡ���򷵻�-1</returns>
        public float GetValue(int index)
        {
            if (CheckIndexValidity(index))
            {
                return values[index];
            }
            return -1f;
        }

        /// <summary>
        /// ������ȫ���
        /// </summary>
        /// <param name="index"></param>
        /// <returns>�Ƿ�ȫ</returns>
        private bool CheckIndexValidity(int index)
        {
            if (index >= values.Length)
                throw new System.IndexOutOfRangeException();
            else
                return true;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
            float radian = 2 * Mathf.PI / values.Length;
            toFill.AddVert(new Vector3(0, 0, 0), color, Vector2.zero);
            for (int i = 0; i < values.Length; i++)
            {
                float y = radius * values[i] * Mathf.Cos(radian * i);
                float x = radius * values[i] * Mathf.Sin(radian * i);
                toFill.AddVert(new Vector3(x, y, 0), color, Vector2.zero);
                if (i < values.Length - 1)
                {
                    toFill.AddTriangle(i + 1, 0, i + 2);
                }
                else
                {
                    toFill.AddTriangle(i + 1, 0, 1);
                }
            }
        }
    }
}