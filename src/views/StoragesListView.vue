<script setup lang="ts">
import type { StorageModel } from '@/models/StorageModel';
import { ref } from 'vue';

const isLoading = ref(true);

const storageData = ref([] as StorageModel[]);

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

async function DeleteStorage(storage:StorageModel)
{
  let sure = confirm("Are you sure to delete '"+storage.id+"'?");
  if (!sure) return;

  let response: Response =await fetch("/api/DeleteStorage", {
    method: "DELETE",
    body: JSON.stringify(storage),
  });
}

</script>

<template>
  <div class="row">
    <div class="col">
      <div v-if="isLoading" class="spinner-border text-primary" role="status">
        <span class="visually-hidden">{{ $t('labels.loading') }}</span>
      </div>
      <div v-else-if="storageData.length <= 0 " class="alert alert-primary" role="alert">
        <div class="d-flex justify-content-between">
          <span>{{ $t('labels.noDataFound') }}</span>
        </div>
      </div>
      <div v-else>
        <table class="table table-bordered">
          <thead>
            <tr>
              <th scope="col">ID</th>
              <th scope="col">Description</th>
              <th scope="col">Placement</th>
              <th scope="col">
                <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#edit-storage-modal">
                  <i class="bi bi-plus-lg"></i> Add Storage
                </button>
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in storageData">
              <td>{{ item.id }}</td>
              <td>{{ item.description }}</td>
              <td>{{ item.placement }}</td>
              <td>
                <div class="btn-group" role="group">
                  <RouterLink :to="'/storage/'+item.id" class="btn btn-outline-primary">Open</RouterLink>
                  <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#edit-storage-modal">
                    <i class="bi bi-pencil"></i>
                  </button>
                  <button type="button" class="btn btn-outline-danger" @click="DeleteStorage(item)">
                    <i class="bi bi-trash"></i>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
