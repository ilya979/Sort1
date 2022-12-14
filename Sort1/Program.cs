using System.Diagnostics;
Stopwatch stopWatch;
ulong rel=0, assign=0;
int[] A, copyA;
// в целях отладки можно вывести результирующий массив
bool printArray = false;

copyA = new int[2];
A = new int[2];

stopWatch = new Stopwatch();

for (int i = 10; i < 100_001; i *= 10) { 
    copyA = createRandomArr(i);
    A = new int[copyA.Length];
    AllSort(A, i);
    Console.WriteLine();
}

int N = copyA.Length;
// сохраним отсортированный массив
for (int i = 0; i < N; i++) copyA[i] = A[i];

swap(ref copyA[N / 3], ref copyA[2 * N / 3]);
Console.WriteLine("---- Почти отсортированный массив ----");
AllSort(A, N);

for(int i = 0; i < N/2; i++) swap(ref A[i], ref A[N - i - 1]);   

for (int i = 0; i < N; i++) copyA[i] = A[i];

Console.WriteLine("---- Обратно отсортированный массив ----");
AllSort(A, N);

void QuickSort(int L, int R)
{
    int m = L-1;
    int P;
    int[] arr = A;
    if (R <= L) return; 
    P = arr[R];
    for(int i=L; i<=R; i++)
    {
        rel++;
        if (arr[i] <= P) swap(ref arr[++m], ref arr[i]);
    }

    QuickSort(L, m-1);
    QuickSort(m+1, R);
}

void MergeSort(int[] arr, int L, int R)
{
    if (R <= L) return;
    int M = (R + L) / 2;
    MergeSort(arr, L, M);
    MergeSort(arr, M+1, R);
    Merge(arr, L, M, R);
}

void Merge(int[] arr, int L, int M, int R)
{
    int[] tArr = new int[R - L + 1];
    int i = L;
    int j = M+1;
    int t = 0;
    while (i <= M && j <= R)
    {
        rel++;
        assign ++;
        if (cmp(arr[i], arr[j]))
            tArr[t++] = arr[j++];
        else
            tArr[t++] = arr[i++];
    }
    while (i <= M)
    {
        assign++;
        tArr[t++] = arr[i++];
    }
    while (j <= R)
    {
        assign++;
        tArr[t++] = arr[j++];
    }
    for (int k = L; k <= R; k++)
    {
        assign++;
        arr[k] = tArr[k - L];
    }
}



// метод последовательно вызывает все необходимые методы сортировки
void AllSort(int[] arr, int N)
{

    StartStat();
    MergeSort(arr, 0, arr.Length-1);
    EndStat("MergeSort", N);

    StartStat();
    QuickSort(0, arr.Length - 1);
    EndStat("QuickSort", N);

    StartStat();
    HeapSort(arr);
    EndStat("HeapSort", N);

    StartStat();
    SelectionSort(arr);
    EndStat("SelectionSort", N);

    StartStat();
    ShellSort(arr);
    EndStat("ShellSort", N);

    StartStat();
    BubbleSort(arr);
    EndStat("BubbleSort", N);

    StartStat();
    InsertionSort(arr);
    EndStat("InsertionSort", N);

    StartStat();
    InsertionShiftSort(arr);
    EndStat("InsertionShiftSort", N);

    StartStat();
    InsertionShiftLogSort(arr);
    EndStat("InsertionShiftLogSort", N);
}

// функции сортировки
void HeapSort(int[] arr)
{
    for (int i = arr.Length / 2 - 1; i >= 0; i--)
        heapify(arr, i, arr.Length);

    for(int j=arr.Length - 1; j >= 0; j--)
    {
        swap(ref arr[j], ref arr[0]);
        heapify(arr, 0, j);
    }

}

void SelectionSort(int[] arr)
{
    int N=arr.Length;
    int maxIndex = findMaxIndex(arr, N);
    for(int i=N-1; i > 0; i--)
    {
        swap(ref arr[i], ref arr[maxIndex]);
        maxIndex = findMaxIndex(arr, i);
    }
}

void ShellSort(int[] arr)
{
    int N=arr.Length;
    for(int gap=N/2; gap>0; gap /= 2)
    {
        for (int i = gap; i < N; i++)
            for (int j = i; j >= gap && cmp(arr[j - gap], arr[j]); j-=gap)
                swap(ref arr[j], ref arr[j - gap]);
    }
}

void BubbleSort(int[] arr)
{
    int N = arr.Length;
    for(int i = 0; i < N; i++)
    {
        for(int j = 0; j < N - i-1; j++)
        {
            if( cmp(arr[j], arr[j + 1]) )
                swap(ref arr[j], ref arr[j + 1]);
        }
    }
}

void InsertionSort(int[] arr)
{
    int N = arr.Length;
    for (int i = 1; i < N - 1; i++)
        for (int j = i - 1; j >= 0 && cmp(arr[j], arr[j + 1]); j--)
        {
            swap(ref arr[j], ref arr[j + 1]);
        }
}

void InsertionShiftSort(int[] arr)
{
    int N = arr.Length;
    int j;
    for (int i = 1; i < N; i++)
    {
        int k = arr[i];
        assign++;
        for (j = i - 1; j >= 0 && cmp(arr[j], k); j--)
        {
            assign++;
            arr[j + 1] = arr[j];
        }
        arr[j + 1] = k;
        assign++;
    }
}

void InsertionShiftLogSort(int[] arr)
{
    int N = arr.Length;
    int j;
    for (int i = 1; i < N; i++)
    {
        int k = arr[i];
        int p = binarySearch(arr, k, 0, i - 1);
        assign++;
        for (j = i - 1; j >= p; j--)
        {
            assign++;
            arr[j + 1] = arr[j];
        }
        assign++;
        arr[j + 1] = k;
    }
}

// вспомогательные методы
int findMaxIndex(int[] arr, int N)
{
    int res = 0;
    for (int i = 0; i < N; i++)
        if (cmp(arr[i], arr[res])) res = i;
    return res;
}

int binarySearch(int[] arr, int key, int low, int high)
{
    rel += 2;
    if (high <= low)
        if (key > arr[low])
            return low + 1;
        else
            return low;
    int mid=(low+high)/2;
    rel++;
    if (key == arr[mid])
        return mid + 1;
    rel++;
    if (key > arr[mid])
        return binarySearch(arr, key, mid + 1, high);
    else
        return binarySearch(arr, key, low, mid - 1);
    }

void swap(ref int a, ref int b)
{
    int tmp = a;
    a = b;
    b = tmp;
    assign += 3;
}

bool cmp(int a, int b)
{
    rel++;
    return (a > b);
}

void heapify(int[] arr, int root, int N)
{
    int L = 2 * root + 1;
    int R = 2 * root + 2;
    int P = root / 2 - 1;
    int X = root;
    if (L < N && cmp(arr[L], arr[X])) X = L;
    if (R < N && cmp(arr[R], arr[X])) X = R;
    if (X == root) return;
    swap(ref arr[X], ref arr[root]);
    heapify(arr, X, N);

}

// сервисные методы
int[] createRandomArr(int N)
{
    int[] res=new int[N];
    Random rnd = new Random();
    for (int i = 0; i < N; i++) res[i] = rnd.Next(0, N*10);
    return res;

}

void StartStat()
{
    for (int j = 0; j < A.Length; j++) A[j] = copyA[j];
    assign = 0;
    rel = 0;
    stopWatch.Restart();
}

void EndStat(string metName, int N)
{
    stopWatch.Stop();
    Console.WriteLine(metName + "(" + N + "): time: " + stopWatch.ElapsedMilliseconds + " ms; Assignment: " + assign + " Relation: " + rel);
    PrintArr(A);
}

void PrintArr(int[] arr)
{
    if(!printArray) return;
    for(int j=0; j < A.Length; j++) Console.Write(arr[j]+"\t");
    Console.WriteLine();
    Console.WriteLine();
    Console.ReadKey();
}
