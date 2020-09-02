import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AppComponent} from './app.component'
import { ProfilerComponent } from './profiler/profiler.component';

const routes: Routes = [
  {
    path: 'profiler',
    component: ProfilerComponent
  },
  {
    path: '',
    redirectTo: 'profiler',
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
