using System.Diagnostics;
Stopwatch stopWatch;
ulong rel=0, assign=0;

int[] A, copyA;
copyA = new int[2];
A = new int[2];

stopWatch = new Stopwatch();

for (int i = 1_000_000; i < 1_000_001; i *= 10) { 
    copyA = createRandomArr(i);
    A = new int[copyA.Length];
    Console.WriteLine();

    StartStat();
    ShellSort(A);
    EndStat("ShellSort", i);

    StartStat();
    InsertionShiftLogSort(A);
    EndStat("InsertionShiftLogSort", i);

    StartStat();
    InsertionShiftSort(A);
    EndStat("InsertionShiftSort", i);

    StartStat();
    InsertionSort(A);
    EndStat("InsertionSort", i);

    StartStat();
    BubbleSort(A);
    EndStat("BubbleSort", i);

}

int N = copyA.Length;
// сохраним отсортированный массив
for (int i = 0; i < N; i++)
{
    copyA[i] = A[i];
}

swap(ref copyA[N / 3], ref copyA[2 * N / 3]);

{
    A = new int[N];
    Console.WriteLine("---- Почти отсортированный массив ----");

    StartStat();
    BubbleSort(A);
    EndStat("BubbleSort", N);

    StartStat();
    InsertionSort(A);
    EndStat("InsertionSort", N);

    StartStat();
    InsertionShiftSort(A);
    EndStat("InsertionShiftSort", N);

    StartStat();
    InsertionShiftLogSort(A);
    EndStat("InsertionShiftLogSort", N);

    StartStat();
    ShellSort(A);
    EndStat("ShellSort", N);

}

for(int i = 0; i < N/2; i++)
{
    swap(ref A[i], ref A[N - i - 1]);   
}
for (int i = 0; i < N; i++)
{
    copyA[i] = A[i];
}

{
    A = new int[N];
    Console.WriteLine("---- Обратно отсортированный массив ----");

    StartStat();
    BubbleSort(A);
    EndStat("BubbleSort", N);

    StartStat();
    InsertionSort(A);
    EndStat("InsertionSort", N);

    StartStat();
    InsertionShiftSort(A);
    EndStat("InsertionShiftSort", N);

    StartStat();
    InsertionShiftLogSort(A);
    EndStat("InsertionShiftLogSort", N);

    StartStat();
    ShellSort(A);
    EndStat("ShellSort", N);

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

void EndStat(string metName, int i)
{
    stopWatch.Stop();
    Console.WriteLine(metName + "(" + i + "): time: " + stopWatch.ElapsedMilliseconds + " ms; Assignment: " + assign + " Relation: " + rel);
}

bool cmp(int a, int b)
{
    rel++;
    return (a > b);
}
