export interface ItemModel
{
    id : string
    count : number
    title : string
    comment : string
}

export interface StorageModel
{
    id : string
    description : string
    items : ItemModel[]
}
