<script setup lang="ts">
import type { StorageModel, ItemModel } from '@/models/StorageModel';

const storageData = defineModel<StorageModel>('storageData');
const itemData = defineModel<ItemModel>('itemData');
if (!itemData)
{
  itemData.value = new ItemModel();
  itemData.value.id = "aaa?";
}
console.log(itemData.id);
</script>

<template>
  <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h1 class="modal-title fs-5" id="exampleModalLabel">Edit item</h1>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <div class="input-group mb-3">
            <span class="input-group-text" id="id-modal-addon">ID</span>
            <input type="text" class="form-control" aria-describedby="id-modal-addon" v-model="itemData.id" >
          </div>
          <div class="input-group mb-3">
            <span class="input-group-text" id="count-modal-addon">Count</span>
            <input type="text" class="form-control" aria-describedby="count-modal-addon" v-model="itemData.count">
          </div>
          <div class="input-group mb-3">
            <span class="input-group-text" id="title-modal-addon">Title</span>
            <input type="text" class="form-control" aria-describedby="title-modal-addon" v-model="itemData.title">
          </div>
          <div class="input-group">
            <span class="input-group-text">Comment</span>
            <textarea class="form-control" v-model="itemData.comment"></textarea>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          <button type="button" class="btn btn-success"><i class="bi bi-check"></i> Save changes</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
