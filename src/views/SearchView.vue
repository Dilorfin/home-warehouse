<script setup lang="ts">
import type { ItemModel, StorageModel } from '@/models/StorageModel';
import { ref } from 'vue';

const isLoading = ref(true);
const searchText = ref('');

const storagesData = ref([] as StorageModel[]);
const filteredData = ref([] as StorageModel[]);
const tab = ref<'storages'|'items'>('storages');

GetStorages();

async function GetStorages()
{
  let response: Response = await fetch("/api/GetStoragesList");

  isLoading.value = false;

  if (response.ok)
  {
    storagesData.value = await response.json() as StorageModel[];
  }
  else
  {
    storagesData.value = [];
  }
  filteredData.value = storagesData.value;
}
function getItemsList()
{
  return filteredData.value.map(s => s.items.map(i => ({ storageId: s.id, itemId: i.id, title: i.title, count: i.count }))).flat();
}
function search()
{
  if (!searchText.value)
  {
    filteredData.value = storagesData.value;
    return;
  }
  filteredData.value = storagesData.value.filter(storage => 
    storage.description.search(searchText.value) > 0
    || storage.id.search(searchText.value) > 0
    || storage.placement.search(searchText.value) > 0
    || storage.items.some(it => it.id.search(searchText.value) > 0 || it.title.search(searchText.value) > 0 || it.comment.search(searchText.value) > 0)
  );
}
</script>

<template>
  <div class="row">
    <div class="col">
      <nav class="nav nav-pills flex-column">
        <a class="nav-link" :class="tab == 'storages' ? 'active':''" @click="tab = 'storages'">Storages</a>
        <a class="nav-link" :class="tab == 'items' ? 'active':''" @click="tab = 'items'">Items</a>
      </nav>
    </div>
    <div class="col-10">
      <div v-if="isLoading" class="spinner-border text-primary" role="status">
        <span class="visually-hidden">{{ $t('labels.loading') }}</span>
      </div>
      <div v-else-if="storagesData.length <= 0 " class="alert alert-primary" role="alert">
        <div class="d-flex justify-content-between">
          <span>{{ $t('labels.noDataFound') }}</span>
        </div>
      </div>
      <div v-else>
        <div class="input-group mb-2">
          <input v-model="searchText" type="text" class="form-control" placeholder="Enter storage ID">
          <button type="button" class="btn btn-outline-secondary" data-bs-toggle="modal" data-bs-target="#exampleModal" @click="searchText = ''; search()">
            <i class="bi bi-x-lg"></i>
          </button>
          <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#exampleModal" @click="search">
            <i class="bi bi-search"></i> Search
          </button>
        </div>
        <table v-if="tab == 'storages'" class="table table-bordered">
          <thead>
            <tr>
              <th scope="col">ID</th>
              <th scope="col">Description</th>
              <th scope="col">Placement</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in filteredData">
              <td>{{ item.id }}</td>
              <td>{{ item.description }}</td>
              <td>{{ item.placement }}</td>
            </tr>
          </tbody>
        </table>
        <table v-else-if="tab == 'items'" class="table table-bordered">
          <thead>
            <tr>
              <th scope="col">StorageID</th>
              <th scope="col">Item Title</th>
              <th scope="col">Count</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in getItemsList()">
              <td>{{ item.storageId }}</td>
              <td>{{ item.title }}</td>
              <td>{{ item.count }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
