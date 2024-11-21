export declare interface ItemModel
{
    id : string
    count : number
    title : string
    comment : string
}

export declare interface StorageModel
{
    id : string
    description : string
    placement : string
    items : ItemModel[]
}
