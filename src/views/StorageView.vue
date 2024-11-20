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
            <input type="text" class="form-control" aria-describedby="id-modal-addon" v-model="itemEditData.id" >
          </div>
          <div class="input-group mb-3">
            <span class="input-group-text" id="count-modal-addon">Count</span>
            <input type="text" class="form-control" aria-describedby="count-modal-addon" v-model="itemEditData.count">
          </div>
          <div class="input-group mb-3">
            <span class="input-group-text" id="title-modal-addon">Title</span>
            <input type="text" class="form-control" aria-describedby="title-modal-addon" v-model="itemEditData.title">
          </div>
          <div class="input-group">
            <span class="input-group-text">Comment</span>
            <textarea class="form-control" v-model="itemEditData.comment"></textarea>
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
