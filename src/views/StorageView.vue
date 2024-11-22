<script setup lang="ts">
import type { StorageModel, ItemModel } from '@/models/StorageModel';
import EditStorageModal from '@/components/EditStorageModal.vue'
import LoadExcelModal from '@/components/LoadExcelModal.vue'
import QrcodeVue from 'qrcode.vue';
import { useRoute } from 'vue-router';
import { ref, watch } from 'vue';
import html2canvas from '@html2canvas/html2canvas';
import printJS from 'print-js';
import * as XLSX from 'xlsx';

const route = useRoute()

const storageId = ref(Array.isArray(route?.params?.id) ? route?.params?.id[0] : route?.params?.id);
const isLoading = ref(true);

const qrReference = ref<HTMLElement>();

const storageData = ref({items:[] as ItemModel[] } as StorageModel);
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
  } as StorageModel;
  return;*/

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

function downloadXLSX()
{
  const workbook = XLSX.utils.book_new();

  const itemsWorksheet = XLSX.utils.json_to_sheet(storageData.value.items);
  XLSX.utils.book_append_sheet(workbook, itemsWorksheet, storageData.value.id);

  const storageInfoWorksheet = XLSX.utils.json_to_sheet([{
    id: storageData.value.id,
    description: storageData.value.description,
    placement: storageData.value.placement,
  }]);
  XLSX.utils.book_append_sheet(workbook, storageInfoWorksheet, 'storage-info');
  
  XLSX.writeFile(workbook, 'storage.xlsx', { compression: true });
}

async function printQR()
{
  const canvas:HTMLCanvasElement = await html2canvas(qrReference.value!, {
    onclone: (_, element) => element.removeAttribute("hidden")
  });
  canvas.toBlob((blob:Blob|null) => {
    const url = URL.createObjectURL(blob!);
    printJS({
      printable: url,
      type: 'image',
      onPrintDialogClose: () => URL.revokeObjectURL(url)
    });
  });
}
async function downloadQR()
{
  const canvas:HTMLCanvasElement = await html2canvas(qrReference.value!, {
    onclone: (_, element) => element.removeAttribute("hidden")
  });
  canvas.toBlob((blob:Blob|null) => {
    const url = URL.createObjectURL(blob!);
    let a = document.createElement('a');
    a.href = url;
    a.download = storageId.value+".png";
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
  });
}
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
  if (!sure) return;
  
  storageData.value.items.splice(index, 1);
  await fetch("/api/UpsertStorage", {
    method: "POST",
    body: JSON.stringify(storageData.value),
  });
}
async function DeleteStorage()
{
  let sure = confirm("Are you sure to delete '"+storageId.value+"'?");
  if (!sure) return;
  
  let response: Response =await fetch("/api/DeleteStorage", {
    method: "DELETE",
    body: JSON.stringify(storageData.value),
  });
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
              <button type="button" class="btn btn-outline-danger" @click="DeleteStorage()">
                <i class="bi bi-trash"></i>
              </button>
              <button type="button" class="btn btn-outline-secondary" @click="downloadXLSX()">
                <i class="bi bi-cloud-download"></i>
              </button>
              <button type="button" class="btn btn-outline-secondary" data-bs-toggle="modal" data-bs-target="#load-excel-modal">
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
              <a class="btn btn-outline-primary" @click="downloadQR()">{{ $t('buttons.downloadQR') }}</a>
              <a class="btn btn-outline-primary" @click="printQR()">Print QR</a>
            </div>
          </div>
          <div ref="qrReference" class="text-center" hidden>
            <h3>{{ storageId }}</h3>
            <qrcode-vue :value="storageId" :size="300" level="Q"></qrcode-vue>
          </div>
        </div>
      </div>
    </div>
  </div>
  <EditStorageModal :modal-id="'edit-storage-modal'" :is-new-item="isNewItem" v-model:item="itemEditData" v-model:storage="storageData"></EditStorageModal>
  <LoadExcelModal :modal-id="'load-excel-modal'" :storageId="storageId" v-model:storage="storageData"></LoadExcelModal>
</template>

<style scoped>

</style>
