<script setup lang="ts">
import type { StorageModel } from '@/models/StorageModel';
import { watch } from 'vue';

const storageData = defineModel<StorageModel>('storage', { required: true });

let saveForCancel = JSON.stringify(storageData.value);
watch(storageData, (value, old)=>{
  saveForCancel = JSON.stringify(value);
});
const props = defineProps({
  modalId: String,
  storageId: String
});

async function saveEditItem()
{
  await fetch("/api/UpsertStorage", {
    method: "POST",
    body: JSON.stringify(storageData.value),
  });
}

function cancelEdit()
{
  storageData.value = JSON.parse(saveForCancel) as StorageModel;
}
</script>

<template>
<div class="modal fade" :id="props.modalId" tabindex="-1" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h1 class="modal-title fs-5" id="exampleModalLabel">{{ $t('labels.editItem') }}</h1>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <div class="input-group mb-3">
          <span class="input-group-text" id="id-modal-addon">ID</span>
          <input type="text" class="form-control" aria-describedby="id-modal-addon" v-model="props.storageId" disabled>
        </div>
        <div class="input-group mb-3">
          <span class="input-group-text" id="count-modal-addon">Description</span>
          <input type="text" class="form-control" aria-describedby="count-modal-addon" v-model="storageData.description">
        </div>
        <div class="input-group mb-3">
          <span class="input-group-text" id="title-modal-addon">Placement</span>
          <input type="text" class="form-control" aria-describedby="title-modal-addon" v-model="storageData.placement">
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" @click="cancelEdit">
          <i class="bi bi-x-lg"></i> {{ $t('labels.cancel') }}
        </button>
        <button type="button" class="btn btn-success" data-bs-dismiss="modal" @click="saveEditItem">
          <i class="bi bi-floppy2-fill"></i> {{ $t('labels.save') }}
        </button>
      </div>
    </div>
  </div>
</div>
</template>

<style scoped>

</style>
