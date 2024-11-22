<script setup lang="ts">
import type { StorageModel } from '@/models/StorageModel';
import { ref } from 'vue';

const isLoading = ref(true);

const storageData = ref([] as StorageModel[]);

var tab = ref<'storages'|'items'>('storages');

GetStorages();

async function GetStorages()
{
  let response: Response = await fetch("/api/GetStoragesList");

  isLoading.value = false;

  if (response.ok)
  {
    storageData.value = await response.json() as StorageModel[];
  }
}
function getItemsList()
{
  return storageData.value.map(s => s.items.map(i => ({ storageId: s.id, itemId: i.id, title: i.title, count: i.count }))).flat();
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
      <div v-else-if="storageData.length <= 0 " class="alert alert-primary" role="alert">
        <div class="d-flex justify-content-between">
          <span>{{ $t('labels.noDataFound') }}</span>
        </div>
      </div>
      <div v-else>
        <table v-if="tab == 'storages'" class="table table-bordered">
          <thead>
            <tr>
              <th scope="col">ID</th>
              <th scope="col">Description</th>
              <th scope="col">Placement</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in storageData">
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
