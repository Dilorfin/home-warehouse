<script setup lang="ts">
import type { StorageModel, ItemModel } from '@/models/StorageModel';
import * as XLSX from 'xlsx';
import { ref } from 'vue';

const storageData = defineModel<StorageModel>('storage', { required: true });

const validationData = ref<{storageId:string, isValid:boolean}[]>([]);

let storages:StorageModel[] = [];

const props = defineProps({
    storageId: String,
    modalId: String
});
async function onFileChanged(event: Event)
{
  const target = event.target as HTMLInputElement;
  if (!target || !target.files) {
    return;
  }

  const data = await target.files[0].arrayBuffer();
  const workbook = XLSX.read(data);

  let storageInfo = XLSX.utils.sheet_to_json<StorageModel>(workbook.Sheets['storage-info']);

  let storageIds:string[] = workbook.SheetNames.filter(sn => !!sn.localeCompare('storage-info'));

  for (const storageId of storageIds) {
    let validation = {
      storageId: storageId,
      isValid: true
    };

    let items = XLSX.utils.sheet_to_json<ItemModel>(workbook.Sheets[storageId]);
    items = items.map(it => {
      validation.isValid = validation.isValid && Number.isInteger(+it.count);
      it.id ??= crypto.randomUUID();
      return it;
    });

    const data = storageInfo.filter(si => si.id === storageId);
    storages.push({
      id: storageId,
      items: items,
      description: data[0]?.description,
      placement: data[0]?.placement,
    } as StorageModel);

    validationData.value.push(validation);
  }
}

async function confirmImport()
{
  await fetch("/api/BatchUpsertStorage", {
    method: "POST",
    body: JSON.stringify(storages),
  });
}

function cancelImport()
{
  // ??
}
</script>

<template>
<div class="modal modal-lg fade" :id="props.modalId" tabindex="-1" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h1 class="modal-title fs-5" id="exampleModalLabel">{{ $t('labels.editItem') }}</h1>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <input type="file" class="form-control" @change="onFileChanged($event)">
        <table v-if="validationData.length > 0" class="table table-bordered mt-2">
          <tbody>
            <tr v-for="(item, index) in validationData">
              <td>{{ item.storageId }}</td>
              <td :class="item.isValid?'text-bg-success':'text-bg-danger'">{{ item.isValid ? 'Valid' : 'Invalid' }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" @click="cancelImport">
          <i class="bi bi-x-lg"></i> {{ $t('labels.cancel') }}
        </button>
        <button type="button" class="btn btn-success" data-bs-dismiss="modal" @click="confirmImport" :disabled="validationData.some(v => !v.isValid)">
          <i class="bi bi-floppy2-fill"></i> {{ $t('labels.save') }}
        </button>
      </div>
    </div>
  </div>
</div>
</template>

<style scoped>

</style>
