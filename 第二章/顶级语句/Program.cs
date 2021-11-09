int i = 1, j = 2;
int w = Add(i, j);
await File.WriteAllTextAsync("e:/1.txt", "hello" + w);
int Add(int i1, int i2)
{
    return i1 + i2;
}