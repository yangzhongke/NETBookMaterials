export function floatItem(items,item)
{
    const indexOfItem = items.indexOf(item);
    if(indexOfItem<0)
    {
        throw "cannot find the item in items";
    }
    else if(indexOfItem==0)
    {
        return;//item is already the first.
    }
    //swap them
    const prevIndex = indexOfItem-1;
    const prevItem = items[prevIndex];
    items[indexOfItem] = prevItem;
    items[prevIndex] = item;
};

export function sinkItem(items,item)
{
    const indexOfItem = items.indexOf(item);
    if(indexOfItem<0)
    {
        throw "cannot find the item in items";
    }
    else if(indexOfItem==items.length-1)
    {
        return;//item is already the last.
    }
    //swap them
    const nextIndex = indexOfItem+1;
    const nextItem = items[nextIndex];
    items[indexOfItem] = nextItem;
    items[nextIndex] = item;
};