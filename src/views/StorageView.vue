<script setup lang="ts">
import type { StorageModel, ItemModel } from '@/models/StorageModel';

import QrcodeVue from 'qrcode.vue';
import { ref, watch } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute()

const storageId = ref(Array.isArray(route?.params?.id) ? route?.params?.id[0] : route?.params?.id);
const isLoading = ref(true);
const storageExists = ref(false);
const storageData = ref({} as StorageModel);
const itemEditData = ref({} as ItemModel);

fetch("/api/GetStorage/?id=" + storageId.value).then(async (response: Response)=>{

  isLoading.value = false;

  /* storageData.value = {
    id: 'thisisid',
    description: 'Description',
    items: [
      {
        id: 'AAAA',
        count: 1,
        title: 'title-test',
        comment: 'comment field'
      },
      {
        id: 'id2',
        count: 42,
        title: 'title-2',
        comment: 'comment 2 field'
      }
    ]
  } as StorageModel;
  */
  if (response.ok)
  {
    storageExists.value = true;
    storageData.value = await response.json() as StorageModel;
  }
  else
  {
    storageExists.value = false;
    //data.value = result["error"];
  }
});
/* ???
watch(
  () => route.params.id,
  (newId, oldId) => {
    storageId.value = Array.isArray(newId) ? newId[0] : newId;
  }
);*/
function openEdit(item:ItemModel)
{
  itemEditData.value = item;
}
</script>

<template>
  <div class="row mt-2">
    <div class="col-md-8 order-2 order-md-1">
      <div v-if="isLoading" class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
      <div v-else-if="!storageExists" class="alert alert-primary" role="alert">
        No data found
        <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
          Add Item
        </button>
      </div>
      <div v-else>
        <table class="table table-bordered">
          <thead>
            <tr>
              <th scope="col">#</th>
              <th scope="col">Title</th>
              <th scope="col">Count</th>
              <th scope="col">Comment</th>
              <th scope="col">
                <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
                  Add Item
                </button>
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in storageData.items">
              <th scope="row">{{ item.id }}</th>
              <td>{{ item.title }}</td>
              <td>{{ item.count }}</td>
              <td>{{ item.comment }}</td>
              <td>
                <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#exampleModal" @click="openEdit(item);">
                  Edit
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div class="col-md order-1 order-md-2">
      <div class="card">
        <div class="card-header">
          <h3 class="card-title">{{ $t('labels.storage') }}: {{ storageId }}</h3>
        </div>
        <div class="card-body">
          {{  storageData.description  }}
        </div>
        <div class="card-footer">
          <div class="d-flex">
            <div class="px-1">
              <qrcode-vue :value="storageId" :size="100"></qrcode-vue>
            </div>
            <div class="btn-group-vertical px-1" role="group" aria-label="Vertical button group">
              <a class="btn btn-outline-primary">{{ $t('buttons.downloadQR') }}</a>
              <a class="btn btn-outline-primary">Print QR</a>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <EditStorageModal v-model:storageData="storageData" v-model:itemData="itemEditData"></EditStorageModal>
</template>

<style scoped>

</style>
