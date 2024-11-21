import './scss/main.scss'

import { createApp } from 'vue'
import { createI18n } from 'vue-i18n'
import App from './App.vue'
import router from './router'

const i18n = createI18n({
    locale: 'ua',
    fallbackLocale: 'en',
    messages: {
      en: {
        language:{
          ua: 'Українська 🇺🇦',
          en: 'English 🇬🇧'
        },
        labels: {
          scanQR: 'Scan QR',
          openStorage: 'Open Storage',
          save: 'Save',
          cancel: 'Cancel',
          storage: 'Storage',
          title: 'Title',
          count: 'Count',
          comment: 'Comment',
          editItem: 'Edit Item',
          loading: 'Loading...',
          noDataFound: 'No items found'
        },
        buttons: {
          scanQR: 'Scan QR',
          openStorage: 'Open',
          addItem: 'Add Item',
          downloadQR: 'Download QR'
        }
      },
      ua: {
        buttons: {
          scanQR: 'Сканувати QR',
          downloadQR: 'Завантажити QR'
        }
      }
    }
});

const app = createApp(App)

app.use(router)
app.use(i18n)

app.mount('#app')

import * as bootstrap from 'bootstrap'
