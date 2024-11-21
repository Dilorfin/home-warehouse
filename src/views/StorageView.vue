<script setup lang="ts">
import type { StorageModel, ItemModel } from '@/models/StorageModel';
import EditStorageModal from '@/components/EditStorageModal.vue'
import QrcodeVue from 'qrcode.vue';
import { useRoute } from 'vue-router';
import { ref, watch } from 'vue';

const route = useRoute()

const storageId = ref(Array.isArray(route?.params?.id) ? route?.params?.id[0] : route?.params?.id);
const isLoading = ref(true);

const storageData = ref({} as StorageModel);
const itemEditData = ref({} as ItemModel); 
let isNewItem = false;

async function OpenStorage()
{
  let response: Response = await fetch("/api/GetStorage/?id=" + storageId.value);

  isLoading.value = false;

  /*storageData.value = {
    id: 'thisisid',
    description: 'Description',
    placement: "placement asd",
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
  } as StorageModel;*/
  
  if (response.ok)
  {
    storageData.value = await response.json() as StorageModel;
  }
  else 
  {
    storageData.value = { id: storageId.value, items:[] as ItemModel[] } as StorageModel;
  }
}

OpenStorage();

watch(
  () => route.params.id,
  async (newId, oldId) => {
    storageId.value = Array.isArray(newId) ? newId[0] : newId;
    await OpenStorage();
  }
);

function openEdit(item:ItemModel | undefined = undefined)
{
  isNewItem = !item;
  itemEditData.value = item ?? { id: crypto.randomUUID() } as ItemModel;
}
async function removeItem(item:ItemModel, index:number)
{
  let sure = confirm("Are you sure to delete '"+item.title+"'?");
  if (sure)
  {
    storageData.value.items.splice(index, 1);
    await fetch("/api/UpsertStorage", {
      method: "POST",
      body: JSON.stringify(storageData.value),
    });
  }
}

</script>

<template>
  <div class="row">
    <div class="col-md-8 order-2 order-md-1">
      <div v-if="isLoading" class="spinner-border text-primary" role="status">
        <span class="visually-hidden">{{ $t('labels.loading') }}</span>
      </div>
      <div v-else-if="storageData.items.length <= 0 " class="alert alert-primary" role="alert">
        <div class="d-flex justify-content-between">
          <span>{{ $t('labels.noDataFound') }}</span>
          <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#edit-storage-modal" @click="openEdit();">
            <i class="bi bi-plus-lg"></i> {{ $t('buttons.addItem') }}
          </button>
        </div>
      </div>
      <div v-else>
        <table class="table table-bordered">
          <thead>
            <tr>
              <th scope="col">{{ $t('labels.title') }}</th>
              <th scope="col">{{ $t('labels.count') }}</th>
              <th scope="col">{{ $t('labels.comment') }}</th>
              <th scope="col">
                <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#edit-storage-modal" @click="openEdit();">
                  <i class="bi bi-plus-lg"></i> {{ $t('buttons.addItem') }}
                </button>
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in storageData.items">
              <td>{{ item.title }}</td>
              <td>{{ item.count }}</td>
              <td>{{ item.comment }}</td>
              <td>
                <div class="btn-group" role="group">
                  <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#edit-storage-modal" @click="openEdit(item);">
                    <i class="bi bi-pencil"></i>
                  </button>
                  <button type="button" class="btn btn-outline-danger" @click="removeItem(item, index);">
                    <i class="bi bi-trash"></i>
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div class="col-md order-1 order-md-2">
      <div class="card">
        <div class="card-header">
          <h3>{{ $t('labels.storage') }}: {{ storageId }}</h3>
        </div>
        <ul class="list-group list-group-flush">
          <li v-if="storageData.description" class="list-group-item">
            <i>Description:</i>
            <div>{{ storageData.description }}</div>
          </li>
          <li v-else class="list-group-item">
            <i>No description</i>
          </li>
          <li v-if="storageData.placement" class="list-group-item">
            <i>Placement:</i> {{ storageData.placement }}
          </li>
          <li v-else class="list-group-item">
            <i>No placement</i>
          </li>
        </ul>
        <div class="card-footer">
          <div class="btn-group" role="group">
              <button type="button" class="btn btn-outline-primary">
                <i class="bi bi-pencil"></i>
              </button>
              <button type="button" class="btn btn-outline-danger">
                <i class="bi bi-trash"></i>
              </button>
              <button type="button" class="btn btn-outline-secondary">
                <i class="bi bi-cloud-download"></i>
              </button>
              <button type="button" class="btn btn-outline-secondary">
                <i class="bi bi-cloud-upload"></i>
              </button>
            </div>
        </div>
      </div>
      <div class="card mt-2">
        <div class="card-body">
          <div class="d-flex">
            <div class="px-1">
              <qrcode-vue :value="storageId" :size="100" level="Q"></qrcode-vue>
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
  <EditStorageModal :modal-id="'edit-storage-modal'" :is-new-item="isNewItem" v-model:item="itemEditData" v-model:storage="storageData"></EditStorageModal>
</template>

<style scoped>

</style>
